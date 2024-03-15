import { makeAutoObservable } from "mobx";
import { Chat } from "../models/Chat";
import axios from "axios";

export default class ChatStore {
    chat: Chat[] | undefined | null;
    error: string = '';

    baseUrl: string = import.meta.env.VITE_BASE_URL;

    constructor() {
        makeAutoObservable(this);
    }

    getChat = async () => {

    }

    getChatDetail = async (id: string) => {
        var token = localStorage.getItem("token");
        axios.get<Chat[]>(this.baseUrl + 'Chat/Chat/' + id, {
            headers: {
                'Authorization': 'Bearer ' + token,
            }
        }).then(response => {
            if (response.status == 200) {
                this.error = '';
                this.chat = response.data;
            } else {
                this.error = 'Error when get request';
                this.chat = null;
            }
        }).catch(error => {
            this.error = error;
        });

    }
}