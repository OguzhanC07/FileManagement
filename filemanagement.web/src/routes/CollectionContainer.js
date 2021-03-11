import React, { useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import IsAuthenticated from "../Pages/Login/IsAuthenticated";
import FolderRoute from "./FolderRoute";
import AuthRoute from "./AuthRoute";

const CollectionContainer = () => {
  const { auth } = useContext(AuthContext);

  return (
    <div>
      {auth.isAuth && <FolderRoute />}
      {!auth.isAuth && !auth.didtryAl && <IsAuthenticated />}
      {!auth.isAuth && auth.didtryAl && <AuthRoute />}
    </div>
  );
};

export default CollectionContainer;
