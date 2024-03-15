import { createContext, useContext } from "react";
import UserStore from "./UserStore";
import ChatStore from "./ChatStore";

interface Store {
    userStore: UserStore;
    chatStore: ChatStore;
}

export const store: Store = {
    userStore: new UserStore(),
    chatStore: new ChatStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}