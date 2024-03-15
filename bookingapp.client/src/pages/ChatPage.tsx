import { HubConnectionBuilder } from "@microsoft/signalr";
import { useEffect } from "react";

function ChatPage() {
    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl(import.meta.env.VITE_SOCKET_URL + "message")
            .build();

        connection.start().then(() => {
            console.log("Connected to SignalR hub!");

            // Send message button click event handler
            const sendButton = document.getElementById("sendButton");
            sendButton!.addEventListener("click", () => {
                const user = (document.getElementById("userInput")! as HTMLInputElement).value;
                const message = (document.getElementById("messageInput")! as HTMLInputElement).value;
                connection.invoke("SendMessage", user, message);
                (document.getElementById("messageInput")! as HTMLInputElement).value = '';
            });

            // Receive message event handler
            connection.on("ReceiveMessage", (user, message) => {
                const msgList = document.getElementById("messagesList");
                const newItem = document.createElement("li");
                newItem.textContent = `${user}: ${message}`;
                msgList!.appendChild(newItem);
            });
        }).catch(err => {
            console.error("SignalR connection error:", err);
        });
    }, []);

    return (
        <div>
            <div>
                <label htmlFor="userInput">Username:</label>
                <input type="text" id="userInput" placeholder="Enter your username" />
            </div>

            <div>
                <label htmlFor="messageInput">Message:</label>
                <input type="text" id="messageInput" placeholder="Type your message" />
                <button id="sendButton">Send</button>
            </div>

            <ul id="messagesList">

            </ul>
        </div>
    );
}

export default ChatPage;