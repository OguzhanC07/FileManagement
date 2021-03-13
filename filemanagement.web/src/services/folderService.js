import axios from "axios";
import apiUrl from "../constants/apiUrl";

export const getfolders = async () => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  try {
    const response = await axios.get(
      apiUrl.baseUrl + `/Folder/GetFoldersByAppUserId/${userInfo.userId}`,
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
      throw new Error("Bağlantı sorunu");
    } else {
      console.log(error);
      throw new Error("Bir şeyler ters gitti!");
    }
  }
};

export const addfolders = async (name, id) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  try {
    if (id > 0 && id !== undefined && !isNaN(id)) {
      const response = await axios.post(
        apiUrl.baseUrl + `/Folder/${id}`,
        { folderName: name },
        {
          headers: {
            Authorization: "Bearer " + userInfo.token,
          },
        }
      );

      return response;
    } else {
      const response = await axios.post(
        apiUrl.baseUrl + `/Folder/`,
        { folderName: name },
        {
          headers: {
            Authorization: "Bearer " + userInfo.token,
          },
        }
      );

      return response;
    }
  } catch (error) {
    if (error.response) {
      console.log(error.response);
    } else if (error.request) {
      console.log(error.response);
      throw new Error("Bağlantı sorunu");
    } else {
      console.log(error);
      throw new Error("Bir şeyler ters gitti!");
    }
  }
};

export const editfolder = async (id, name) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  try {
    const response = await axios.put(
      apiUrl.baseUrl + `/Folder/${id}`,
      { id, folderName: name },
      {
        headers: {
          Authorization: "Bearer " + userInfo.token,
        },
      }
    );
    return response;
  } catch (error) {
    if (error.response) {
      throw new Error("Folder name must be alphanumeric characters");
    } else if (error.request) {
      console.log(error.response);
      throw new Error("Bağlantı sorunu");
    } else {
      console.log(error);
      throw new Error("Bir şeyler ters gitti!");
    }
  }
};

export const deletefolder = async (id) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  try {
    const response = await axios.delete(apiUrl.baseUrl + `/Folder/${id}`, {
      headers: {
        Authorization: "Bearer " + userInfo.token,
      },
    });

    return response;
  } catch (error) {
    if (error.response) {
      console.log(error.response);
      throw new Error(error.response);
    } else if (error.request) {
      console.log(error.response);
      throw new Error("Bağlantı sorunu");
    } else {
      console.log(error);
      throw new Error("Bir şeyler ters gitti!");
    }
  }
};
