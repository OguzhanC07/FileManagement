import React from "react";

const ImgDisplay = (props) => {
  return (
    <div className="outer">
      <img src={props.file} height="400px" alt="your" />
    </div>
  );
};

export default ImgDisplay;
