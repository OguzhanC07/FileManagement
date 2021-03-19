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
      throw new Error("Username or password is not valid.");
    } else if (error.request) {
      throw new Error("Connection Problem");
    } else {
      throw new Error("Something went wrong");
    }
  }
};
