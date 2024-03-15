import { Outlet } from 'react-router-dom';
import './App.css';
import { useStore } from './stores/store';
import { useEffect } from 'react';


function App() {
    const { userStore } = useStore();

    useEffect(() => {
        userStore.getUserByToken().then(() => {
            if (userStore.error == '') {
                console.log(userStore.user?.id);
                console.log(userStore.user?.fullName);
                console.log(userStore.user?.email);
                // window.location.replace('/chat');
            } else {
                console.log(userStore.error);
            }
        });
    }, [])

    return (
        <>
            <Outlet />
        </>
    )
}

export default App;