import axios from "axios";
import apiUrl from "../constants/apiUrl";

export const signin = async (userName, password) => {
  try {
    const response = await axios.post(apiUrl.baseUrl + "/user", {
      userName,
      password,
    });
    console.log(response.status);
  } catch (error) {}
};
