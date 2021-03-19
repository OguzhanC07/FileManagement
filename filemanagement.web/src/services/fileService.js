import axios from "axios";
import apiUrl from "../constants/apiUrl";

export const getfiles = async (id) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  try {
    const response = await axios.get(apiUrl.baseUrl + `/File/GetFiles/${id}`, {
      headers: {
        Authorization: "Bearer " + userInfo.token,
      },
    });
    return response;
  } catch (error) {
    if (error.response) {
      return error.response;
    } else if (error.request) {
      console.log(error.response);
      throw new Error("Connection problem");
    } else {
      console.log(error);
      throw new Error("Something went wrong");
    }
  }
};

export const getsinglefile = async (id) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  try {
    const response = await axios.get(apiUrl.baseUrl + `/File/${id}`, {
      responseType: "blob",
      headers: {
        Authorization: "Bearer " + userInfo.token,
      },
    });

    return response;
  } catch (error) {
    if (error.response) {
      return error.response;
    } else if (error.request) {
      console.log(error.response);
      throw new Error("Connection problem");
    } else {
      console.log(error);
      throw new Error("Something went wrong");
    }
  }
};

export const editfile = async (id, fileName) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  try {
    const response = await axios.put(
      apiUrl.baseUrl + `/File/${id}`,
      { id, fileName },
      {
        headers: {
          Authorization: "Bearer " + userInfo.token,
        },
      }
    );
    return response;
  } catch (error) {
    if (error.response) {
      return error.response;
    } else if (error.request) {
      console.log(error.response);
      throw new Error("Connection problem");
    } else {
      console.log(error);
      throw new Error("Something went wrong");
    }
  }
};

export const deletefile = async (id) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  try {
    const response = await axios.delete(apiUrl.baseUrl + `/File/${id}`, {
      headers: {
        Authorization: "Bearer " + userInfo.token,
      },
    });
    return response;
  } catch (error) {
    if (error.response) {
      return error.response;
    } else if (error.request) {
      console.log(error.response);
      throw new Error("Connection problem");
    } else {
      console.log(error);
      throw new Error("Something went wrong");
    }
  }
};

export const uploadfile = async (id, data) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  const formData = new FormData();
  for (const file of data) {
    formData.append("formFiles", file, file.name);
  }

  try {
    const response = await axios.post(
      apiUrl.baseUrl + `/File/UploadFile/${id}`,
      formData,
      {
        headers: {
          Authorization: "Bearer " + userInfo.token,
        },
      }
    );

    return response;
  } catch (error) {
    if (error.response) {
      console.log(error.response);
    } else if (error.request) {
      console.log(error.response);
      throw new Error("Connection problem");
    } else {
      console.log(error);
      throw new Error("Something went wrong.");
    }
  }
};
