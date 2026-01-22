import Box from "@mui/material/Box";
import { sxStyles } from "../constants";

export function Loader() {
  return (
    <Box sx={sxStyles.centeredLoader}>
      <span>Loading...</span>
    </Box>
  );
}