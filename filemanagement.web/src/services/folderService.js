import axios from "axios";
import apiUrl from "../constants/apiUrl";

export const getfolders = async (id) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  const language = localStorage.getItem("i18nextLng");

  if (id > 0 && !isNaN(id) && id !== undefined) {
    try {
      const response = await axios.get(
        apiUrl.baseUrl + `/Folder/GetSubFoldersByFolderId/${id}`,
        {
          headers: {
            Authorization: "Bearer " + userInfo.token,
            "Accept-Language": language,
          },
        }
      );

      return response;
    } catch (error) {
      if (error.response) {
        throw new Error(error.response.data.message);
      } else if (error.request) {
        throw new Error("connection");
      } else {
        throw new Error("wentWrong");
      }
    }
  } else {
    try {
      const response = await axios.get(
        apiUrl.baseUrl + `/Folder/GetFoldersByAppUserId/${userInfo.userId}`,
        {
          headers: {
            Authorization: "Bearer " + userInfo.token,
            "Accept-Language": language,
          },
        }
      );
      return response;
    } catch (error) {
      if (error.response) {
        throw new Error(error.response.data.message);
      } else if (error.request) {
        throw new Error("connection");
      } else {
        throw new Error("wentWrong");
      }
    }
  }
};

export const addfolders = async (name, id) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  const language = localStorage.getItem("i18nextLng");

  try {
    if (id > 0 && id !== undefined && !isNaN(id)) {
      const response = await axios.post(
        apiUrl.baseUrl + `/Folder/${id}`,
        { folderName: name },
        {
          headers: {
            Authorization: "Bearer " + userInfo.token,
            "Accept-Language": language,
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
            "Accept-Language": language,
          },
        }
      );

      return response;
    }
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

export const editfolder = async (id, name) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  const language = localStorage.getItem("i18nextLng");
  try {
    const response = await axios.put(
      apiUrl.baseUrl + `/Folder/${id}`,
      { id, folderName: name },
      {
        headers: {
          Authorization: "Bearer " + userInfo.token,
          "Accept-Language": language,
        },
      }
    );
    return response;
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

export const downloadfolder = async (id) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  const language = localStorage.getItem("i18nextLng");

  try {
    const response = await axios.get(
      apiUrl.baseUrl + `/Folder/DownloadFolder/${id}`,
      {
        responseType: "blob",
        headers: {
          Authorization: "Bearer " + userInfo.token,
          "Accept-Language": language,
        },
      }
    );

    return response;
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

export const deletefolder = async (id) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  const language = localStorage.getItem("i18nextLng");

  try {
    const response = await axios.delete(apiUrl.baseUrl + `/Folder/${id}`, {
      headers: {
        Authorization: "Bearer " + userInfo.token,
        "Accept-Language": language,
      },
    });

    return response;
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
