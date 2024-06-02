import { createApp } from 'vue'
import App from './App.vue'

// import { ElButton, ElLink, ElRow, ElCol, ElInput, ElDivider, ElCard, ElTooltip, ElSwitch, } from 'element-plus'
import 'element-plus/packages/theme-chalk/src/base.scss'

const app = createApp(App);

// app.component(ElButton.name, ElButton)
// app.component(ElRow.name, ElRow)
// app.component(ElCol.name, ElCol)
// app.component(ElInput.name, ElInput)
// app.component(ElDivider.name, ElDivider)
// app.component(ElLink.name, ElLink)
// app.component(ElCard.name, ElCard)
// app.component(ElTooltip.name, ElCard)
// app.component(ElSwitch.name, ElSwitch)

app.mount('#app')
