import axios from "axios";
const apiBaseUrl = "http://localhost:5000";
const instance = axios.create({
    baseURL: apiBaseUrl
});


// localStorage.setItem("username", "Magenta");
// localStorage.setItem("token", "f9fd77e70a874c15a80f98843a9665d5");

// export const getUserToken = () => ({name: localStorage.getItem("username"), token: localStorage.getItem("token")});
// const authorizedRequestConfig = { headers: { Authorization: getUserCredentias().token }}

// export const getPost = (postId) => instance.get(`/posts/${postId}`).then(result => result.data);

// export const getUser = (userId) => instance.get(`/users/${userId}`).then(result => result.data);

// export const postComment = (postId, content) => instance.post(`/posts/${postId}/comments`, {content: content}, authorizedRequestConfig).then(result => result.data);
export const userSignUp = (username, password) => instance.post("/users/signup", {username: username, password: password}).then(result => result.data);
export const userSignIn = (username, password) => instance.post("/users/signin", {username: username, password: password}).then(result => result.data);
