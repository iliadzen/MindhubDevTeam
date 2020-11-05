import axios from "axios";
const apiBaseUrl = "http://localhost:5000";
const instance = axios.create({
    baseURL: apiBaseUrl
});

const authorizedRequestConfig = { headers: { Authorization: "Bearer "+localStorage.getItem("accessToken") }}

// User
export const userSignUp = (username, password) => instance.post("/users/signup", {username: username, password: password}).then(result => result.data);
export const userSignIn = (username, password) => instance.post("/users/signin", {username: username, password: password}).then(result => result.data);
export const userEdit = (username, password) => instance.put("/users/self", {username: username, password: password}, authorizedRequestConfig).then(result => result.data);
export const getSelf = () => instance.get("/users/self", authorizedRequestConfig).then(result => result.data);

// Tracker
export const getTrackers = () => instance.get("/trackers", authorizedRequestConfig).then(result => result.data);
export const getTracker = (trackerId) => instance.get(`/trackers/${trackerId}`, authorizedRequestConfig).then(result => result.data);
export const editTracker = (trackerName, trackerId) => instance.put(`/trackers/${trackerId}`, {title: trackerName}, authorizedRequestConfig).then(result => result.data);
export const deleteTracker = (trackerId) => instance.delete(`/trackers/${trackerId}`, authorizedRequestConfig).then(result => result.data);
