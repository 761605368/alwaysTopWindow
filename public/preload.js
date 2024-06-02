const { execSync } = require("child_process")
const path = require('path');
const iconv = require('iconv-lite');
const { clipboard } = require('electron');
const fs = require('fs')

window.copy_text = text => {
  clipboard.writeText(text);
}

window.get_app_base_name = app => path.basename(app, utools.isWindows() ? '.exe' : '.app')

// MacOS下返回可执行文件对应的app路径
window.get_mac_app_path = (file_path) => {
  if (!utools.isMacOS()) return file_path;
  return file_path?.match(/(.*?\/.*?.app)\/Contents\/MacOS\//)?.[1];
}

window.exec_file = path => execFile(path);

const get_unpack_file_path = ExeFile => {
  if (utools.isDev()) {
    return path.join(__dirname, 'bin', ExeFile)
  } else {
    return path.join(__dirname.replace(/([a-zA-Z0-9\-]+\.asar)/, '$1.unpacked'), 'bin', ExeFile)
  }
}

const compile_csharp_file = script => {
  // utools.showNotification('插件初始化中...')
  var compile_path;
  var script_path = get_unpack_file_path(script)
  var script_dir = path.dirname(script_path)
  var dotnet3 = 'C:\\Windows\\Microsoft.NET\\Framework\\v3.5\\csc.exe'
  var dotnet4 = 'C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\csc.exe'
  if (fs.existsSync(dotnet4)) {
    compile_path = dotnet4
  } else if (fs.existsSync(dotnet3)) {
    compile_path = dotnet3
  } else {
    utools.showNotification('插件初始化失败，需要安装.net framework 4.0')
    utools.outPlugin();
    return
  }
  try {
    execSync(`pushd ${script_dir} && ${compile_path} ${script}`)
  } catch (error) {
    utools.showNotification('插件初始化失败，请下载upx版进行手动安装')
    utools.outPlugin();
  }
}

const windowmanager = get_unpack_file_path('WindowManager.exe')
if (!fs.existsSync(windowmanager) && utools.isWindows()) compile_csharp_file('WindowManager.cs')

const run_command = cmd => {
  let codec = utools.isWindows() ? 'gb18030' : 'utf8'
  try {
    let stdout = execSync(cmd, { windowsHide: true, encoding: 'buffer' })
    return iconv.decode(stdout, codec)
  } catch (stderr) {
    utools.showNotification(iconv.decode(stderr, codec))
  }
}

window.winman_get_window_list = () => {
  let window_list
  if (utools.isWindows()) {
    window_list = JSON.parse(run_command(`${windowmanager} windows`))
  } else if (utools.isMacOS()) {
    window_list = JSON.parse(run_command(String.raw`osascript -e 'set json to "["
tell application "System Events"
	set visibleProcesses to (every process whose visible is true)
	repeat with aProcess in visibleProcesses
		set processName to name of aProcess
		tell aProcess
			set pid to unix id
			set ppath to do shell script "ps -p " & pid & " -o comm="
			set bid to bundle identifier
			repeat with aWindow in (every window)
				set windowName to name of aWindow
				set json to json & " {\"process\": \"" & processName & "\", \"title\": \"" & windowName & "\",\"id\": \"" & pid & "\",\"bid\": \"" & bundle identifier & "\", \"path\":\"" & ppath & "\"},"
			end repeat
		end tell
	end repeat
end tell
set json to text 1 through -2 of json -- remove last comma
set json to json & "]"
return json'`)).filter(x => x.process !== 'app_mode_loader');
  }
  console.log(window_list);

  window_list.sort((a, b) => a.id > b.id ? 1 : a.id === b.id ? 0 : -1);
  return window_list
}

window.winman_activate_window = (window) => {
  if (utools.isWindows()) {
    run_command(`${windowmanager} activate id ${window.id}`)
  } else if (utools.isMacOS()) {
    run_command(`osascript -e 'set theProcessName to "${window.process}"
set theWindowTitle to "${window.title}"
tell application "System Events"
	set theProcess to (first process whose name is theProcessName)
	if theProcess exists then
		tell theProcess
			set theWindow to (first window whose name is theWindowTitle)
			if theWindow exists then
				perform action "AXRaise" of theWindow
			else
				display dialog "Window not found"
			end if
		end tell
	else
		display dialog "Process not found"
	end if
end tell'`)
  }
  utools.hideMainWindow()
  utools.outPlugin()
}

window.winman_get_window_status = window => {
  if (utools.isWindows()) {
    return run_command(`${windowmanager} status id ${window.id}`)
  }
}

window.winman_set_window_status = (window, action, value = '') => {
  if (utools.isWindows()) {
    console.log(window, action, value);
    return run_command(`${windowmanager} ${action} id ${window.id} ${value}`)
  }
}
