import React, { useContext, useEffect } from "react";
import Loader from "react-loader-spinner";
import { AuthContext, TRYAL } from "../../context/AuthContext";
import "./Login.css";

const IsAuthenticated = () => {
  const { dispatch } = useContext(AuthContext);

  useEffect(() => {
    const myInterval = setInterval(() => {
      var userInfo = JSON.parse(localStorage.getItem("userinfo"));
      if (userInfo !== null) {
        var dateOne = new Date();
        var dateTwo = new Date(userInfo.expirationDate);
        if (dateOne - dateTwo > 0) {
          localStorage.removeItem("userinfo");
          dispatch({
            type: TRYAL,
            user: {
              token: "",
              userId: "",
              expirationDate: 0,
              isAuth: false,
              didtryAl: true,
            },
          });
        } else {
          dispatch({
            type: TRYAL,
            user: {
              token: userInfo.token,
              userId: userInfo.userId,
              expirationDate: userInfo.expirationDate,
              isAuth: true,
              didtryAl: true,
            },
          });
        }
      } else {
        dispatch({
          type: TRYAL,
          user: {
            token: "",
            userId: "",
            expirationDate: 0,
            isAuth: false,
            didtryAl: true,
          },
        });
      }
    }, 3000);
    return () => {
      clearInterval(myInterval);
    };
  });

  return (
    <div className="loader">
      <Loader
        type="Puff"
        color="#00BFFF"
        height={100}
        width={100}
        timeout={3000}
      />
    </div>
  );
};

export default IsAuthenticated;
