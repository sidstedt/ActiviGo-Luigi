import React, { useMemo, useState } from "react";
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
];

export default function MonthlyBookingsLineChart({
  months,
  bookingsPerMonth,
  selectedYear,
  palette = DEFAULT_PALETTE,
}) {
  const [color, setColor] = useState(palette[0]);
  const { xData, yData } = useMemo(() => {
    const xs = Array.isArray(months) ? months.slice(0, 12) : [];
    const ys = Array.isArray(bookingsPerMonth)
      ? bookingsPerMonth.slice(0, 12)
      : [];
    while (xs.length < 12) xs.push("");
    while (ys.length < 12) ys.push(0);
    return { xData: xs, yData: ys };
  }, [months, bookingsPerMonth]);

  const yMax = useMemo(() => {
    const max = Math.max(0, ...yData);
    // Ensure we always have some vertical space even if all values are 0
    if (max === 0) return 1;
    // Small headroom (10%) for aesthetics
    return Math.max(1, Math.ceil(max * 1.1));
  }, [yData]);

  const monthPairs = useMemo(
    () => xData.map((m, i) => ({ month: m, value: yData[i] ?? 0 })),
    [xData, yData]
  );

  return (
    <Stack direction="column" spacing={2}>
      <LineChart
        height={320}
        xAxis={[{ data: xData, label: "Månad", scaleType: "point" }]}
        yAxis={[{ min: 0, max: yMax, label: "Bokningar" }]}
        series={[
          {
            data: yData,
            label: `Bokningar ${selectedYear ?? ""}`.trim(),
            color,
            showMark: true,
            area: true,
            valueFormatter: (v) => `${v}`,
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
