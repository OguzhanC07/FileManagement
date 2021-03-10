import React from "react";
import { BrowserRouter as Switch, Route } from "react-router-dom";
import Login from "../Pages/Login/Login";

const AuthRoute = () => {
  return (
    <Switch>
      <Route path="/" component={Login} />
    </Switch>
  );
};

export default AuthRoute;
