import { User } from "./User";

export interface Chat {
    id:         number;
    senderId:   number;
    receiverId: number;
    text:       string;
    createdAt:  Date;
}