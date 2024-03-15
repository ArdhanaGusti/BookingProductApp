import { RouteObject, createBrowserRouter } from "react-router-dom";
import App from "../App";
import LoginPage from "../pages/LoginPage";
import ChatPage from "../pages/ChatPage";
import DetailChatPage from "../pages/DetailChatPage";

const routes: RouteObject[] = [
    {
        path: "/",
        element: <App />,
        children: [
            { path: "", element: <LoginPage /> },
            { path: "/chat", element: <ChatPage /> },
            { path: "/chat/:userId",  element: <DetailChatPage /> },
        ],
    },
];

export const router = createBrowserRouter(routes, {
    basename: import.meta.env.BASE_URL,
});