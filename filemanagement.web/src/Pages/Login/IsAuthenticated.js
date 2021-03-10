import React, { useContext } from "react";
import Loader from "react-loader-spinner";
import { AuthContext, TRYAL } from "../../context/AuthContext";

const IsAuthenticated = () => {
  const { dispatch } = useContext(AuthContext);

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

  return (
    <Loader
      type="Puff"
      color="#00BFFF"
      height={100}
      width={100}
      timeout={3000}
    />
  );
};

export default IsAuthenticated;
