<template>
    <div class="container">
        <div class="main">
            <div class="resetbox">
                <div class="resetbox-in">
                    <div class="header">重置密码</div>
                    <div class="form-wrapper">

                        <!-- Grouping phone input and send code button -->
                        <div class="input-group">
                            <input type="text" v-model="phoneNumber" placeholder="手机号" class="input-item" />
                            <div class="btn-send-code" @click="sendCode">发送验证码</div>
                        </div>

                        <div class="input-wrapper">
                            <input type="text" v-model="verificationCode" placeholder="验证码" class="input-item" />
                        </div>
                        <div class="input-wrapper">
                            <input type="password" v-model="newPassword" placeholder="新密码" class="input-item" />
                        </div>
                        <div class="input-wrapper">
                            <input type="password" v-model="confirmPassword" placeholder="确认密码" class="input-item" />
                        </div>
                        <div class="btn2" @click="resetPassword">Reset</div>
                        <div class="btn-passwordlogin" @click="$router.push('/login')">账号密码登录→</div>
                    </div>
                    <p v-if="resetError" class="error-message">{{ resetError }}</p>
                </div>
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
                phoneNumber: '',
                verificationCode: '',
                newPassword: '',
                confirmPassword: '',
                resetError: '',
                event: '验证'  // 默认事件类型
            };
        },

        methods: {

            async checkPhoneExists() {
                try {
                    const response = await api.apiClientWithoutToken.post(`/api/account/check-phone?phoneNumber=${encodeURIComponent(this.phoneNumber)}`);
                    return response.status === 200;
                } catch (error) {
                    console.error(error);
                    this.resetError = '检查手机号时发生错误，请稍后重试。';
                    return false;
                }
            }

            ,

            async sendCode() {
                // 验证手机号码格式
                if (!/^\d{11}$/.test(this.phoneNumber)) {
                    this.resetError = '请输入11位有效的手机号码。';
                    return;
                }

                //检查手机号是否存在
                const phoneExists = await this.checkPhoneExists();
                if (!phoneExists) {
                    this.resetError = '手机号不存在。';
                    return;
                }

                try {
                    // 此处是发送验证码的API
                    const response = await api.apiClientWithoutToken.post('/api/sms/sendsms', {
                        phoneNumber: this.phoneNumber,
                        event: this.event
                    });

                    if (response.status === 200) {
                        ElMessage.success('验证码已发送');
                    } else {
                        this.resetError = response.data.msg || '发送验证码失败，请重试。';
                    }
                } catch (error) {
                    console.error(error);
                    this.resetError = '发送验证码时发生错误，请稍后重试。';
                }
            },


            async verifyCode() {
                try {
                    const response = await api.apiClientWithoutToken.post('/api/sms/verifycode', {
                        phoneNumber: this.phoneNumber,
                        event: this.event,
                        verificationCode: this.verificationCode
                    });

                    return response.status === 200;
                } catch (error) {
                    console.error(error);
                    this.resetError = '验证验证码时发生错误，请稍后重试。';
                    return false;
                }
            },

            async resetPassword() {
                // 验证手机号码格式
                if (!/^\d{11}$/.test(this.phoneNumber)) {
                    this.resetError = '请输入11位有效的手机号码。';
                    return;
                }

                // 验证验证码是否为空
                if (this.verificationCode.trim() === '') {
                    this.resetError = '验证码不能为空。';
                    return;
                }

                const codeValid = await this.verifyCode();
                if (!codeValid) {
                    this.resetError = '验证码无效或已过期。';
                    return;
                }

                // 验证新密码是否为空
                if (this.newPassword.trim() === '') {
                    this.resetError = '新密码不能为空。';
                    return;
                }

                // 验证两次输入的密码是否一致
                if (this.newPassword !== this.confirmPassword) {
                    this.resetError = '两次输入的密码不一致。';
                    return;
                }

                try {
                    const response = await api.apiClientWithoutToken.post('/api/account/reset-password', {
                        phoneNumber: this.phoneNumber,
                        Password: this.newPassword
                    });

                    if (response.status === 200) {
                        ElMessage.success('密码已重置成功');
                        // Redirect to the login page
                        this.$router.push('/login');
                    } else {
                        this.resetError = response.data.msg || '重置密码失败，请重试。';
                    }
                } catch (error) {
                    console.error(error);
                    this.resetError = '重置密码时发生错误，请稍后重试。';
                }
            }
        },
        name: 'ForgottenPassword'
    }
</script>


<style scoped>
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
        justify-content: center;
        align-items: center;
    }

    .main {
        display: flex;
        justify-content: space-evenly;
    }

    .resetbox {
        display: flex;
        width: 600px;
        height: 560px;
        position: relative;
        top: 20%;
        left: 10%;
        box-shadow: 0 12px 16px 0 rgba(0, 0, 0, 0.24), 0 17px 50px 0 rgba(0, 0, 0, 0.19);
        background-image: linear-gradient(to left, #c6d6f7c1, rgba(255, 255, 255, 0.902));
    }

    .resetbox-in {
        width: 360px;
        border-radius: 15px;
        padding: 0 50px;
        position: absolute;
        left: 50%;
        top: 10%;
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
        margin-top: 20px;
    }

    .form-wrapper {
        width: 100%;
        padding-top: 30px;
    }

    .input-group {
        display: flex;
        align-items: center;
        margin-bottom: 25px;
    }

    .input-item {
        flex: 1;
        padding: 12px;
        border: 1px solid rgb(128, 125, 125);
        border-radius: 10px;
        font-size: 15px;
        outline: none;
        margin-bottom: 4px; /* 增加间距 */
        margin-right: 10px; /* Adds space between the input and the button */
    }

    .btn-send-code {
        padding: 12px;
        background-color: #6a8cdcc1;
        color: #ffffff;
        border-radius: 10px;
        cursor: pointer;
        font-size: 15px;
        white-space: nowrap; /* Prevents button text from wrapping */
    }

    .btn2 {
        text-align: center;
        padding: 8px;
        width: 100%;
        margin-top: 40px;
        background-color: #6a8cdcc1;
        color: #ffffff;
        border-radius: 10px;
        cursor: pointer;
        font-size: 20px;
    }

        .btn2:hover {
            background-color: #284da0c1;
        }

    .btn-passwordlogin {
        position: absolute;
        top: 420px;
        left: 25%;
        background-color: transparent;
        border-radius: 15px;
        display: inline-block;
        cursor: pointer;
        color: grey;
        font-family: Arial;
        font-size: 15px;
        padding: 16px 31px;
        text-decoration: none;
        margin: 10px 20px;
    }

    .title {
        position: absolute;
        top: 650px;
        left: 820px;
        font-size: 22px;
        color: #284da0c1;
    }

        .title:hover {
            font-size: 21px;
            transition: all 0.4s ease-in-out;
            cursor: pointer;
        }

    .error-message {
        color: rgba(209, 41, 41, 0.815);
        text-align: center;
        margin-top: -80px;
    }
</style>
