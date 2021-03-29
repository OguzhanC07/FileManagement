import React, { useState } from "react";
import { Modal, Icon } from "semantic-ui-react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useTranslation } from "react-i18next";

import { getsinglefile } from "../services/fileService";
import ImgDisplay from "./ImgDisplay";
import PdfDisplay from "./PdfDisplay";
import VideoDisplay from "./VideoDisplay";
import "../styles/fileViewer.css";

const Viewer = (props) => {
  const [file, setFile] = useState(null);
  const [open, setOpen] = useState(false);
  const [isDisabled, setIsDisabled] = useState(false);
  const { t } = useTranslation();

  const viewHandler = async (e) => {
    e.preventDefault();
    let response;
    let problem = "";
    try {
      setIsDisabled(true);
      response = await getsinglefile(props.id);
      setIsDisabled(false);
    } catch (error) {
      problem = error;
      setIsDisabled(false);
    }

    if (problem !== "") {
      toast.error(t("viewer.viewError") + t("responseErrors.wentWrong"));
    } else {
      let base64;
      switch (props.type) {
        case "pdf":
          base64 = await convertBase64(response.data);
          setFile(base64);
          setOpen(true);
          break;
        case "img":
          base64 = await convertBase64(response.data);
          setFile(base64);
          setOpen(true);
          break;
        case "video":
          setFile(URL.createObjectURL(response.data));
          setOpen(true);
          break;
        default:
          break;
      }
    }
  };

  const convertBase64 = (blob) => {
    return new Promise((resolve, reject) => {
      const fileReader = new FileReader();
      fileReader.readAsDataURL(blob);

      fileReader.onload = () => {
        resolve(fileReader.result);
      };

      fileReader.onerror = (error) => {
        reject(error);
      };
    });
  };

  return (
    <span>
      <Icon
        name="external square alternate"
        disabled={isDisabled}
        onClick={(e) => {
          viewHandler(e);
        }}
      />
      <ToastContainer
        position="top-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
      />
      <Modal
        onOpen={() => setOpen(true)}
        basic
        closeIcon
        open={open}
        onClose={() => {
          setOpen(false);
        }}
      >
        {props.type === "pdf" && <PdfDisplay file={file} />}
        {props.type === "img" && <ImgDisplay file={file} />}
        {props.type === "video" && <VideoDisplay file={file} />}
      </Modal>
    </span>
  );
};

export default Viewer;
