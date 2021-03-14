import axios from "axios";
import apiUrl from "../constants/apiUrl";

export const uploadfile = async (id, data) => {
  const userInfo = JSON.parse(localStorage.getItem("userinfo"));
  const formData = new FormData();
  console.log(data);
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
      throw new Error("Bağlantı sorunu");
    } else {
      console.log(error);
      throw new Error("Bir şeyler ters gitti!");
    }
  }
};
