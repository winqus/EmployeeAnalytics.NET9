import Box from "@mui/material/Box";
import { sxStyles } from "../constants";

export function ErrorOccured({ error }: { error: Error }) {
  return (
    <Box sx={sxStyles.centeredError}>
      An error has occurred: {error.message}
    </Box>
  );
}