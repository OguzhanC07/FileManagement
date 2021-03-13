import axios from "axios";

export const uploadfile = (id, data) => {
  const formData = new FormData();
  console.log(data);
  formData.append("id", id);
  for (const file of data) {
    formData.append("formFiles", file, file.name);
  }

  console.log(formData);
};
