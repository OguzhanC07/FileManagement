import React, { createContext, useReducer } from "react";

import apiUrl from "../constants/apiUrl";

export const SIGNIN = "SIGNIN";

export const AuthContext = createContext();

const authReducer = (state, action) => {
  switch (action.type) {
    case SIGNIN:
      return {
        token: action.token,
        userId: action.userId,
      };
    default:
      return state;
  }
};

const initialState = {
  token: "",
  userId: "",
  errorMessage: "",
};

const AuthContextProvider = (props) => {
  const [auth, dispatch] = useReducer(authReducer, initialState);

  return (
    <AuthContext.Provider value={{ auth, dispatch }}>
      {props.children}
    </AuthContext.Provider>
  );
};

export default AuthContextProvider;
