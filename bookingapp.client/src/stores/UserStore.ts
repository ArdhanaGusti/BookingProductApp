import { makeAutoObservable } from "mobx";
import axios, { AxiosError } from "axios";
import { Token } from "../models/Token";
import { User } from "../models/User";

export default class UserStore {
    loading: boolean = false;
    token: Token | undefined | null;
    error: string = '';

    userError: string = '';
    user: User | undefined | null;

    baseUrl: string = import.meta.env.VITE_BASE_URL;

    constructor() {
        makeAutoObservable(this);
    }

    loginUser = async (email: string, password: string) => {
        this.loading = true;
        try {
            const response = await axios.post<Token>(this.baseUrl + 'Auth/Login', {
                email: email,
                password: password
            });
            if (response.status == 200) {
                this.token = response.data;
                this.error = '';
                localStorage.setItem("token", this.token.token);
                localStorage.setItem("expired", this.token.expired.toString());
            }
        } catch (error) {
            const err = error as AxiosError;
            this.error = err.message;
        } finally {
            this.loading = false;
        }
    };

    getUserByToken = async () => {
        var expired = new Date(Date.parse(localStorage.getItem("expired")!));
        var now = new Date();

        if (now < expired) {
            var token = localStorage.getItem("token");
            const response = await axios.get<User>(this.baseUrl + 'User/Me', {
                headers: {
                    'Authorization': 'Bearer ' + token,
                }
            });
            if (response.status == 200) {
                this.error = '';
                this.user = response.data;
            }
        } else {
            localStorage.removeItem("expired");
            localStorage.removeItem("token");
            this.error = "Token is expired";
        }
    }
}