<template>
    <div class="container">
        <div style="height: 40px">
            置顶窗口靠前:<el-switch v-model="data.topWindow"/>
            显示可见:<el-switch v-model="data.isVisible"/>
            <el-button style="margin-left: 20px;" plain @click="init()">
                刷新
            </el-button>
            <el-button style="margin-left: 20px;" plain @click="dialogVisible = true">
                取消所有置顶
            </el-button>
            <div>
                <el-dialog v-model="dialogVisible" title="Tips" width="300px" draggable>
                    <span>是否取消所有置顶</span>
                    <template #footer>
                        <div class="dialog-footer">
                            <el-button @click="dialogVisible = false">Cancel</el-button>
                            <el-button type="primary" @click="closeAllTopWindows">
                                Confirm
                            </el-button>
                        </div>
                    </template>
                </el-dialog>
            </div>
        </div>
        <div class="item" v-for="item in windowsList" >
            <el-card>
                置顶:<el-switch @change="changeWindowTop(item)" v-model="item.isTopMost"/>
                <div style="margin-top: 2px" v-show="data.isVisible">可见:<el-switch @change="changeWindowVisible(item)" v-model="item.isVisible"/></div>
                <img
                        :src="item.icon ? item.icon : './default.png'"
                        style="width: 100%"
                        :title="item.title"
                />
            </el-card>
        </div>
    </div>
</template>

<script setup>
    import {defineProps, defineEmits, reactive, ref, watch, computed, onMounted, onUnmounted} from 'vue'

    import keyboard from "keyboardjs"

    // 窗口数据
    let windowsList = reactive(window.winman_get_window_list());

    // 显示取消置顶的对话框
    let dialogVisible = ref(false);

    // 控制显示顺序的数据
    const data = reactive({
        topWindow: true, // 置顶窗口在前面
        isVisible: false, //不显示可见性

    })

    watch(() => data.topWindow, () => {
        console.log("置顶窗口在前面", data)
        if (data.topWindow) {
            windowsList.sort((a, b) => -(a.isTopMost ? 1 : a.isTopMost === b.isTopMost ? 1 : -1));
        } else {
            windowsList.sort((a, b) => a.id > b.id ? 1 : a.id === b.id ? 0 : -1);
        }
    })

    // 取消所有置顶窗口
    function closeAllTopWindows() {
        windowsList.forEach(win => {
            changeWindowTop(win);
            win.isTopMost = false;
        })
        dialogVisible.value = false;
    }

    // 获取图标
    function cache_window_ico(win) {
        win.icon = ""
        if (!win.path || win.path === "程序路径获取失败，需要管理员权限") return win
        let app_path = window.get_mac_app_path(win.path)
        if (!app_path) return win
        let app_name = window.get_app_base_name(app_path)
        let app_icon = utools.dbStorage.getItem(app_name)
        if (!app_icon) {
            app_icon = utools.getFileIcon(app_path)
            utools.dbStorage.setItem(app_name, app_icon)
        }
        win.app = app_name
        win.icon = app_icon

        return win;
    }

    // 获取状态：是否置顶，透明度
    function getStatus(win) {
        let status = window.winman_get_window_status(win);
        if (!status) return
        status = JSON.parse(status);
        console.log(status)
        win.isVisible = !!status.isVisible; //可见
        win.isTopMost = !!status.IsTopMost; //置顶
        win.isEnable = status.isEnable; //禁用
        win.opacity = status.opacity; //透明度0-255
        return win;
    }

    // 加载的时候获取图标
    onMounted(async () => {
        windowsList.forEach(win => {
            cache_window_ico(win);
            getStatus(win);
        })
        console.log(windowsList);
    })

    function init() {
        console.log("init")
        windowsList = reactive(window.winman_get_window_list());
        windowsList.forEach(win => {
            cache_window_ico(win);
            getStatus(win);
        })
        console.log(windowsList);
    }

    function changeWindowVisible(win) {

        let cmd = win.isVisible ? 'show' : 'hide';
        window.winman_set_window_status(win, cmd);
    }

    function changeWindowTop(win) {
        if (win.isTopMost) {
            // 先隐藏
            changeWindowVisible({...win, isVisible: false})
            // 再显示
            changeWindowVisible({...win, isVisible: true})
        }

        let cmd = win.isTopMost ? "top" : "untop";
        let result =  window.winman_set_window_status(win, cmd)
        console.log("设置窗口状态", cmd, result)
    }

    // watch(() => windows, () => {
    //     console.log("监听到windows数据发生变化，重新获取窗口数据")
    //     windows.valueOf = window.listAllWindows();
    // }, {deep: true, immediate: false})

    // const props = defineProps({
    //   windows: [{id: 1}],
    // })

    // const state = reactive({ content: props.content })
    //
    // watch(() => props.content, () => {
    //   state.content = props.content
    // })
    //
    // watch(() => props.path, () => {
    //   state.path = props.path
    //
    //   if (props.path) {
    //     console.log('basic path', props.path, getFileDirectory(props.path));
    //     marked.setOptions({
    //       baseUrl: getFileDirectory(props.path)
    //     })
    //   } else {
    //     console.log("props path is empty");
    //     marked.setOptions({
    //       baseUrl: null
    //     })
    //   }
    // }, {
    //   immediate: true
    // })


    // save and save as
    // const emits = defineEmits(['save'])


    // function handleSave() {
    //     if (props.path === "") {
    //         handleSaveAs()
    //     } else {
    //         emits('save', props.path, state.content);
    //     }
    // }

    // keyboard
    // keyboard.bind("mod > s", () => {
    //     handleSave()
    // });
    // keyboard.bind("mod + shift > s", () => {
    //     handleSaveAs()
    // });

</script>

<style scoped>
    .item {
        float: left;
        flex: auto;
        width: 120px;

    }


</style>
