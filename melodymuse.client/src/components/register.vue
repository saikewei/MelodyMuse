<template>
    <div class="container">
        <div class="main">
            <div class="registerbox">
                <div class="registerbox-in">
                    <div class="header">MelodyMuse</div>
                    <div class="form-wrapper">
                        <div class="input-wrapper">
                            <!--<span class="iconfont icon-account"></span>-->
                            <input type="text" name="username" placeholder="用户名" class="input-item" v-model="username">
                        </div>

                        <div class="input-wrapper">
                            <!--<span class="iconfont icon-account"></span>-->
                            <input type="tel" name="phonenumber" placeholder="手机号码" class="input-item" v-model="phonenumber">
                            <!-- 新增发送验证码按钮 -->
                            <div class="send-code-btn" @click="sendCode">验证码</div>
                        </div>

                        <!-- 新增验证码输入框 -->
                        <div class="input-wrapper">
                            <input type="text" name="verificationCode" placeholder="请输入验证码" class="input-item" v-model="verificationCode">
                        </div>

                        <div class="input-wrapper">
                            <!--<span class="iconfont icon-key"></span>-->
                            <input type="password" name="password" placeholder="密码" class="input-item" v-model="password">
                        </div>
                        <div class="input-wrapper">
                            <!--<span class="iconfont icon-key"></span>-->
                            <input type="password" name="confirmPassword" placeholder="确认密码" class="input-item" v-model="confirmPassword">
                        </div>
                        <div class="btn2" @click="register">Register</div>

                    </div>
                    <p v-if="registerError" class="error-message">{{ registerError }}</p>
                </div>
            </div>
            <!-- 右侧盒子 -->
            <div class="background">
                <div class="title">欢迎来到MelodyMuse！请注册您的新账户</div>
            </div>
        </div>
    </div>
</template>

<script>
    import { ElMessage } from 'element-plus';
    import api from '../api/http.js'

    export default {
        data() {
            return {
                username: '',
                phonenumber: '',
                password: '',
                confirmPassword: '',
                verificationCode: '',   // 新增：保存验证码的输入
                codeSent: false,        // 新增：标记验证码是否已发送
                registerError: '',
                event: '注册'  // 默认事件类型

            };
        },
        methods: {
            async checkPhoneExists() {
                try {
                    const response = await api.apiClientWithoutToken.post(`/api/account/check-phone?phoneNumber=${encodeURIComponent(this.phonenumber)}`);
                    return response.status === 200;
                } catch (error) {
                    //console.error(error);
                    //this.registerError = '检查手机号时发生错误，请稍后重试。';
                    return false;
                }
            }

            ,

            // 发送验证码的逻辑
            async sendCode() {
                // 验证手机号码格式
                if (!/^\d{11}$/.test(this.phonenumber)) {
                    this.registerError = '请输入11位有效的手机号码。';
                    return;
                }

                //检查手机号是否存在
                const phoneExists = await this.checkPhoneExists();
                if (phoneExists) {
                    this.registerError = '手机号已经存在。';
                    return;
                }

                try {
                    // 此处是发送验证码的API
                    const response = await api.apiClientWithoutToken.post('/api/sms/sendsms', {
                        phoneNumber: this.phonenumber,
                        event: this.event
                    });

                    if (response.status === 200) {
                        alert('验证码已发送');
                        this.codeSent = true;  // 更新：验证码已发送
                        this.registerError = '';
                    } else {
                        this.registerError = response.data.msg || '发送验证码失败，请重试。';
                    }
                } catch (error) {
                    console.error(error);
                    this.registerError = '发送验证码时发生错误，请稍后重试。';
                }

            },
            async verifyCode() {
                try {
                    const response = await api.apiClientWithoutToken.post('/api/sms/verifycode', {
                        phoneNumber: this.phonenumber,
                        event: this.event,
                        verificationCode: this.verificationCode
                    });

                    return response.status === 200;
                } catch (error) {
                    console.error(error);
                    this.registerError = '验证验证码时发生错误，请稍后重试。';
                    return false;
                }
            },
            async register() {

                // 验证用户名是否为空
                if (this.username.trim() === '') {
                    this.registerError = '用户名不能为空。';
                    return;
                }

                // 验证手机号码长度和格式
                if (!/^\d{11}$/.test(this.phonenumber)) {
                    console.log('啵啵啵啵啵啵啵啵啵啵啵啵:', this.phonenumber); // 打印发送的数据
                    this.registerError = '请输入11位有效的手机号码。';
                    return;
                }

                // 验证密码是否为空
                if (this.password.trim() === '') {
                    this.registerError = '密码不能为空。';
                    return;
                }

                // 验证两次输入的密码是否一致
                if (this.password !== this.confirmPassword) {
                    this.registerError = '两次输入的密码不一致。';
                    return;
                }


                try {
                    const response = await api.apiClientWithoutToken.post('/api/account/register', {
                        userphone: this.phonenumber,
                        username: this.username,
                        password: this.password
                    });

                    if (response.status === 200) {
                        ElMessage.success('注册成功!');
                        // Redirect to the login page
                        this.$router.push('/login');
                    } else {
                        this.registerError = response.data.msg || '注册失败，请再次尝试';
                    }
                } catch (error) {
                    // 检查错误对象，获取详细信息
                    if (error.response && error.response.data && error.response.data.msg) {
                        //console.log('啵啵啵啵啵啵啵啵啵啵啵啵:', response); // 打印发送的数据
                        this.registerError = `错误：${error.response.data.msg}`;

                    } else {
                        this.registerError = '注册失败，请重试。';
                    }
                }
            }
        },
        name: "Reg"
    }
</script>

<style scoped>
    /* 保持与登录页面相同的样式 */
    html, body {
        height: 100%;
        margin: 0;
    }

    .container {
        height: 100%;
        position: absolute;
        left: 0;
        top: 0;
        width: 100%;
        background-image: linear-gradient(to right, #6a8cdcc1, white);
        display: flex;
        justify-content: left;
        align-items: center;
    }

    .main {
        display: flex;
        justify-content: space-evenly;
    }

    .registerbox {
        display: flex;
        width: 55%;
        height: 68%;
        position: absolute;
        top: 14%;
        left: 24%;
        box-shadow: 0 12px 16px 0 rgba(0, 0, 0, 0.24), 0 17px 50px 0 rgba(0, 0, 0, 0.19);
        background-image: linear-gradient(to left, #c6d6f7c1, rgba(255, 255, 255, 0.902));
    }

    .registerbox-in {
        width: 42%;
        border-radius: 15px;
        padding: 0 50px;
        position: absolute;
        left: 25%;
        top: -5%;
        transform: translateX(-50%);
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .header {
        font-size: 36px;
        color: #284da0c1;
        font-weight: bolder;
        text-align: center;
        line-height: 80px;
        margin-top: 30px;
    }

    .form-wrapper {
        width: 100%;
        padding-top: 30px;
    }

    .input-wrapper {
        display: flex;
        align-items: center;
        margin-bottom: 12px;
    }

    .input-item {
        flex: 1;
        display: block;
        width: calc(100% - 20px);
        margin-left: 10px;
        padding: 12px;
        border: 1px solid rgb(128, 125, 125);
        border-radius: 10px;
        font-size: 15px;
        outline: none;
    }

        .input-item::placeholder {
            text-transform: uppercase;
        }

    .btn2 {
        text-align: center;
        padding: 8px;
        width: 100%;
        margin-top: 20px;
        background-color: #6a8cdcc1;
        color: #ffffff;
        border-radius: 10px;
        cursor: pointer;
        font-size: 20px;
    }

        .btn2:hover {
            background-color: #284da0c1;
        }

        .btn2:active {
            position: relative;
            top: 1px;
        }

    .error-message {
        color: rgba(209, 41, 41, 0.815);
        text-align: center;
        margin-top: 10px;
        font-size: 14px;
    }

    .background {
        width: 500px;
        justify-content: center;
        align-items: flex-end;
        background-image: url('src/assets/logo1.png');
        background-size: cover;
    }

    .title {
        position: absolute;
        top: 70%;
        left: 48%;
        font-size: 22px;
        color: #284da0c1;
    }

        .title:hover {
            font-size: 20px;
            transition: all 0.4s ease-in-out;
            cursor: pointer;
        }

    .iconfont {
        font-family: "iconfont" !important;
        font-size: 20px;
        font-style: normal;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        height: 22px;
        color: #4E655D;
        margin-right: 10px;
        margin-top: 3px;
    }

    input:-webkit-autofill {
        box-shadow: 0 0 0 1000px rgba(255, 255, 255, 0) inset !important;
        -webkit-text-fill-color: #445b53 !important;
        transition: background-color 5000s ease-in-out 0s;
    }

        input:-webkit-autofill::first-line {
            font-size: 15px;
            font-weight: bold;
        }
    /* 发送验证码按钮样式 */
    .send-code-btn {
        margin-left: 10px;
        background-color: #6a8cdcc1;
        color: #ffffff;
        border: none;
        border-radius: 10px;
        cursor: pointer;
        padding: 12px 16px;
        font-size: 15px;
        transition: background-color 0.3s ease;
    }

        .send-code-btn:hover {
            background-color: #284da0c1;
        }

    /* 确保验证码输入框与其他输入框样式一致 */
    .verification-code-input {
        width: 100%;
    }
</style>