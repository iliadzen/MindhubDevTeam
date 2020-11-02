import {BrowserRouter, Switch, Route, Redirect} from "react-router-dom";
import UserSignUp from "./User/UserSignUp";
import UserSignIn from "./User/UserSignIn";
import UserApp from "./User/UserApp";

const welcomeText = () => (
    <div>
        <h1>WARNING: Lol Exception</h1>
        <ul>
            <li><a href="users/signup">Sign Up</a></li>
            <li><a href="users/signin">Sign In</a></li>
            <li><a href="users/signout">Sign Out</a></li>
            <li><a href="users/self">Profile</a></li>
        </ul>
    </div>
);

const App = () => {
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/users/signup">
                    {localStorage.getItem("accessToken") ? <Redirect to="/"/> : <UserSignUp />}
                </Route>
                <Route path="/users/signin">
                    {localStorage.getItem("accessToken") ? <Redirect to="/"/> : <UserSignIn />}
                </Route>
                <Route path="/users/self">
                    {!localStorage.getItem("accessToken") ? <Redirect to="/users/signin"/> : <UserApp />}
                </Route>
                {/* <Route path="/users/signout">
                    {localStorage.getItem("accessToken") ? localStorage.removeItem("accessToken") : []}
                </Route> */}
                <Route path="/" component={welcomeText}/>
            </Switch>
        </BrowserRouter>
    )
}

export default App