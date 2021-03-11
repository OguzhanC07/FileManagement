import React, { createContext, useReducer } from "react";

export const SIGNIN = "SIGNIN";
export const TRYAL = "TRYAL";
export const LOGOUT = "LOGOUT";

export const AuthContext = createContext();

const authReducer = (state, action) => {
  switch (action.type) {
    case SIGNIN:
      localStorage.setItem("userinfo", JSON.stringify(action.user));
      return {
        token: action.user.token,
        userId: action.user.userId,
        expirationDate: action.user.expirationDate,
        isAuth: true,
      };
    case TRYAL: {
      return {
        token: action.user.token,
        userId: action.user.userId,
        expirationDate: action.user.expirationDate,
        isAuth: action.user.isAuth,
        didtryAl: action.user.didtryAl,
      };
    }
    case LOGOUT: {
      localStorage.removeItem("userinfo");
      return {
        ...initialState,
        didtryAl: true,
      };
    }
    default:
      return state;
  }
};

const initialState = {
  token: "",
  userId: "",
  expirationDate: 0,
  isAuth: false,
  didtryAl: false,
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
