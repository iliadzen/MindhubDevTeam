import axios from "axios";
const apiBaseUrl = "http://localhost:5000";
const instance = axios.create({
    baseURL: apiBaseUrl
});

const authorizedRequestConfig = { headers: { Authorization: "Bearer "+localStorage.getItem("accessToken") }}

export const userSignUp = (username, password) => instance.post("/users/signup", {username: username, password: password}).then(result => result.data);
export const userSignIn = (username, password) => instance.post("/users/signin", {username: username, password: password}).then(result => result.data);
export const getSelf = () => instance.get("/users/self", authorizedRequestConfig).then(result => result.data);
