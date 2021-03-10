import React, { useContext } from "react";
import { BrowserRouter as Router } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import IsAuthenticated from "../Pages/Login/IsAuthenticated";
import FolderRoute from "./FolderRoute";
import AuthRoute from "./AuthRoute";

const CollectionContainer = () => {
  const { auth } = useContext(AuthContext);

  return (
    <Router>
      {auth.isAuth && <FolderRoute />}
      {!auth.isAuth && !auth.didtryAl && <IsAuthenticated />}
      {!auth.isAuth && auth.didtryAl && <AuthRoute />}
    </Router>
  );
};

export default CollectionContainer;
