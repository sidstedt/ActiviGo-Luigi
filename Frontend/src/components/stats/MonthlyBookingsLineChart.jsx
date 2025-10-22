import React, { useState } from "react";
import { LineChart } from "@mui/x-charts/LineChart";
import Stack from "@mui/material/Stack";
import ToggleButton from "@mui/material/ToggleButton";
import ToggleButtonGroup from "@mui/material/ToggleButtonGroup";

const DEFAULT_PALETTE = [
  "#4e79a7",
  "#f28e2c",
  "#e15759",
  "#76b7b2",
  "#59a14f",
  "#edc949",
  "#af7aa1",
  "#ff9da7",
  "#9c755f",
  "#bab0ab",
];

export default function MonthlyBookingsLineChart({
  months,
  bookingsPerMonth,
  selectedYear,
  palette = DEFAULT_PALETTE,
}) {
  const [color, setColor] = useState(palette[0]);

  return (
    <Stack direction="column" spacing={2}>
      <LineChart
        height={300}
        xAxis={[{ data: months, label: "Månad" }]}
        series={[
          {
            data: bookingsPerMonth,
            label: `Bokningar ${selectedYear ?? ""}`,
            color,
          },
        ]}
      />
      <ToggleButtonGroup
        value={color}
        exclusive
        onChange={(e, next) => next && setColor(next)}
      >
        {palette.map((value) => (
          <ToggleButton key={value} value={value} sx={{ p: 1 }}>
            <div
              style={{
                width: 15,
                height: 15,
                backgroundColor: value,
                borderRadius: 3,
              }}
            />
          </ToggleButton>
        ))}
      </ToggleButtonGroup>
    </Stack>
  );
}
