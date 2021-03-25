import axios from "axios";
import apiUrl from "../constants/apiUrl";

export const signin = async (userName, password) => {
  let language = localStorage.getItem("i18nextLng");
  try {
    const response = await axios.post(
      apiUrl.baseUrl + "/user",
      {
        userName,
        password,
      },
      {
        headers: {
          "Accept-Language": language,
        },
      }
    );

    return response.data;
  } catch (error) {
    if (error.response) {
      throw new Error(error.response.data.message);
    } else if (error.request) {
      throw new Error("connection");
    } else {
      throw new Error("wentWrong");
    }
  }
};
