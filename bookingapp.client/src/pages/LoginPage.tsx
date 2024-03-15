import { useState } from "react";
import { useStore } from "../stores/store";

function LoginPage() {
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const { userStore } = useStore();

    var handleSubmit = (event: any) => {
        event.preventDefault();
        userStore.loginUser(email, password).then(() => {
            if (userStore.error == '') {
                console.log(userStore.token?.expired);
                console.log(userStore.token?.token);
                window.location.replace('/chat');
            } else {
                console.log(userStore.error);
            }
        });
    }

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <input type="text" name="email" value={email} onChange={(e) => { setEmail(e.target.value) }} placeholder="Email" />
                <input type="text" name="password" value={password} onChange={(e) => { setPassword(e.target.value) }} placeholder="Password" />
                <button type="submit">Login</button>
            </form>
        </div>
    );
};

export default LoginPage;