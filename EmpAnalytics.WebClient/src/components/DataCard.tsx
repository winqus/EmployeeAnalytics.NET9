import { Box, Typography } from "@mui/material";
import { sxStyles } from "../constants";
import type { ComponentProps } from "react";

export function DataCard({ title, children }: { title: string } & ComponentProps<'div'>) {
  return (
    <Box>
      <Typography variant="h5" sx={sxStyles.sectionHeading}>
        {title}
      </Typography>
      {children}
    </Box>
  );
}