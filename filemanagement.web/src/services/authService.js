import axios from "axios";
import apiUrl from "../constants/apiUrl";

export const signin = async (userName, password) => {
  try {
    const response = await axios.post(apiUrl.baseUrl + "/user", {
      userName,
      password,
    });

    return response.data;
  } catch (error) {
    if (error.response) {
      throw new Error("Kullanıcı adı veya şifre yanlış");
    } else if (error.request) {
      throw new Error("Bağlantı sorunu");
    } else {
      throw new Error("Bir şeyler ters gitti!");
    }
  }
};
