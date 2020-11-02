import {BrowserRouter, Switch, Route, Redirect} from "react-router-dom";
import UserSignUp from "./User/UserSignUp";
import UserSignIn from "./User/UserSignIn";

const welcomeText = () => (<h1>WARNING: Lol Exception</h1>);

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
                {/* <Route path="/users/self" component={Post}/> */}
                {/* <Route path="/users/self" component={Post}/> */}
                <Route path="/" component={welcomeText}/>
            </Switch>
        </BrowserRouter>
    )
}

export default App