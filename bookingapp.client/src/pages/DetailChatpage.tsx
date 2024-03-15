import { HubConnectionBuilder } from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { useStore } from "../stores/store";
import { useParams } from "react-router-dom";
import { observer } from "mobx-react";

function DetailChatPage() {
    const { userId } = useParams();
    const { chatStore, userStore } = useStore();
    const [message, setMessage] = useState<string>('');
    const connection = new HubConnectionBuilder()
        .withUrl(import.meta.env.VITE_SOCKET_URL + "message")
        .build();

    useEffect(() => {
        getChatDetail();

        connection.start().then(() => {
            console.log("Connected to SignalR hub!");

            // Receive message event handler
            connection.on("ReceiveMessage", (user) => {
                if (userStore.user?.id == user) {
                    getChatDetail();
                }
            });
        }).catch(err => {
            console.error("SignalR connection error:", err);
        });

    }, [])

    function getChatDetail() {
        chatStore.getChatDetail(userId!);
    }

    const handleSubmit = async (event: any) => {
        event.preventDefault();
        console.log(userStore.user?.id);
        connection.start().then(() => {
            connection.invoke("SendMessage", userStore.user?.id?.toString(), userId, message);
        });
    }

    return (
        <div>
            <ul>
                {chatStore.chat?.map((item) => (
                    <li key={item.id}>
                        <p>{item.senderId}: {item.text}</p>
                    </li>
                ))}
            </ul>

            <form onSubmit={handleSubmit}>
                <input type="text" value={message} onChange={(e) => { setMessage(e.target.value) }} />
                <button type="submit">Send</button>
            </form>
        </div>
    )
}

export default observer(DetailChatPage);