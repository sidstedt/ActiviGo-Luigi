import React, { useMemo, useState } from "react";
import { PieChart, pieArcLabelClasses } from "@mui/x-charts/PieChart";
import { useDrawingArea } from "@mui/x-charts/hooks";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { styled } from "@mui/material/styles";
import ToggleButton from "@mui/material/ToggleButton";
import ToggleButtonGroup from "@mui/material/ToggleButtonGroup";
import {
  Tableau10,
  hexToRgba,
  getActivityName,
  isOccurrenceCompleted,
} from "../../utils/statistics";

const StyledText = styled("text")(({ theme }) => ({
  fill: theme.palette.text.primary,
  textAnchor: "middle",
  dominantBaseline: "central",
  fontSize: 16,
}));
const PieCenterLabel = ({ children }) => {
  const { width, height, left, top } = useDrawingArea();
  return (
    <StyledText x={left + width / 2} y={top + height / 2}>
      {children}
    </StyledText>
  );
};

const statusColors = { completed: "#4caf50", upcoming: "#e57373" };

export default function ActivityDistributionPie({ activityOccurrences }) {
  const [view, setView] = useState("activity"); // 'activity' | 'status'

  const data = useMemo(() => {
    const countsByActivity = new Map();
    let totalAll = 0;
    let totalCompleted = 0;
    let totalUpcoming = 0;

    for (const occ of activityOccurrences || []) {
      const name = getActivityName(occ);
      const entry = countsByActivity.get(name) || {
        total: 0,
        completed: 0,
        upcoming: 0,
      };
      entry.total += 1;
      totalAll += 1;
      if (isOccurrenceCompleted(occ)) {
        entry.completed += 1;
        totalCompleted += 1;
      } else {
        entry.upcoming += 1;
        totalUpcoming += 1;
      }
      countsByActivity.set(name, entry);
    }

    const activities = Array.from(countsByActivity.entries());

    const innerActivity = activities.map(([label, v], idx) => ({
      id: label,
      label: `${label}:`,
      value: v.total,
      percentage: totalAll > 0 ? (v.total / totalAll) * 100 : 0,
      color: Tableau10[idx % Tableau10.length],
      completed: v.completed,
      upcoming: v.upcoming,
    }));

    const innerStatus = [
      {
        id: "completed",
        label: "Genomförd:",
        value: totalCompleted,
        percentage: totalAll > 0 ? (totalCompleted / totalAll) * 100 : 0,
        color: statusColors.completed,
      },
      {
        id: "upcoming",
        label: "Ej genomförd:",
        value: totalUpcoming,
        percentage: totalAll > 0 ? (totalUpcoming / totalAll) * 100 : 0,
        color: statusColors.upcoming,
      },
    ];

    const outerStatusActivity = [];
    const opacities = [0.35, 0.45, 0.55, 0.65, 0.75, 0.85, 0.95];
    activities.forEach(([label, v], idx) => {
      const op = opacities[idx % opacities.length];
      outerStatusActivity.push({
        id: `${label}-completed`,
        label: `${label}:`,
        value: v.completed,
        percentage:
          innerStatus[0].value > 0
            ? (v.completed / innerStatus[0].value) * 100
            : 0,
        color: hexToRgba(statusColors.completed, op),
      });
      outerStatusActivity.push({
        id: `${label}-upcoming`,
        label: `${label}:`,
        value: v.upcoming,
        percentage:
          innerStatus[1].value > 0
            ? (v.upcoming / innerStatus[1].value) * 100
            : 0,
        color: hexToRgba(statusColors.upcoming, op),
      });
    });

    return { totalAll, innerActivity, innerStatus, outerStatusActivity };
  }, [activityOccurrences]);

  return (
    <div style={{ marginBottom: "2rem" }}>
      <h2>Fördelning av tillfällen</h2>
      <ToggleButtonGroup
        color="primary"
        size="small"
        value={view}
        exclusive
        onChange={(e, v) => v && setView(v)}
        sx={{ mb: 1 }}
      >
        <ToggleButton value="activity">Visa per aktivitet</ToggleButton>
        <ToggleButton value="status">Visa per status</ToggleButton>
      </ToggleButtonGroup>
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          gap: 3,
          flexWrap: "wrap",
          height: 360,
        }}
      >
        {view === "activity" ? (
          <>
            <PieChart
              series={[
                {
                  innerRadius: 80,
                  outerRadius: 170,
                  data: data.innerActivity,
                  valueFormatter: ({ value, dataIndex }) => {
                    const d = data.innerActivity[dataIndex] || {};
                    const pct =
                      data.totalAll > 0
                        ? ((value / data.totalAll) * 100).toFixed(0)
                        : 0;
                    const comp = d.completed ?? 0;
                    const up = d.upcoming ?? 0;
                    return (
                      `${value} av ${data.totalAll} (${pct}%)\nGenomförd: ${comp}` +
                      (up ? `\nEj genomförd: ${up}` : "")
                    );
                  },
                  highlightScope: { fade: "global", highlight: "item" },
                  highlighted: { additionalRadius: 2 },
                  cornerRadius: 3,
                },
              ]}
              sx={{ [`& .${pieArcLabelClasses.root}`]: { fontSize: "12px" } }}
              hideLegend
              width={420}
              height={340}
            >
              <PieCenterLabel>Aktivitet</PieCenterLabel>
            </PieChart>

            <div style={{ minWidth: 260 }}>
              <Typography variant="subtitle1" gutterBottom>
                Aktiviteter
              </Typography>
              <div>
                {data.innerActivity.map((d) => (
                  <div
                    key={d.id}
                    style={{
                      display: "flex",
                      alignItems: "center",
                      margin: "4px 0",
                    }}
                    title={`Totalt: ${d.value}\nGenomförd: ${d.completed}\n${
                      d.upcoming ? `Ej genomförd: ${d.upcoming}` : ""
                    }`}
                  >
                    <div
                      style={{
                        width: 14,
                        height: 14,
                        background: d.color,
                        borderRadius: 3,
                        marginRight: 8,
                      }}
                    />
                    <div
                      style={{
                        flex: 1,
                        minWidth: 140,
                        overflow: "hidden",
                        textOverflow: "ellipsis",
                        whiteSpace: "nowrap",
                      }}
                    >
                      {String(d.id)}
                    </div>
                    <strong style={{ marginLeft: 8 }}>{d.value}</strong>
                  </div>
                ))}
              </div>
            </div>
          </>
        ) : (
          <PieChart
            series={[
              {
                innerRadius: 80,
                outerRadius: 170,
                data: data.innerStatus,
                arcLabel: (item) =>
                  `${item.id || item.label} (${Number(
                    item.percentage || 0
                  ).toFixed(0)}%)`,
                valueFormatter: ({ value }) =>
                  `${value} av ${data.totalAll} (${
                    data.totalAll > 0
                      ? ((value / data.totalAll) * 100).toFixed(0)
                      : 0
                  }%)`,
                highlightScope: { fade: "global", highlight: "item" },
                highlighted: { additionalRadius: 2 },
                cornerRadius: 3,
              },
              {
                innerRadius: 170,
                outerRadius: 190,
                data: data.outerStatusActivity,
                arcLabel: (item) =>
                  `${(item.id || "").split("-")[0]} (${Number(
                    item.percentage || 0
                  ).toFixed(0)}%)`,
                arcLabelRadius: 150,
                highlightScope: { fade: "global", highlight: "item" },
                highlighted: { additionalRadius: 2 },
                cornerRadius: 3,
              },
            ]}
            sx={{ [`& .${pieArcLabelClasses.root}`]: { fontSize: "12px" } }}
            legend={{
              position: { vertical: "middle", horizontal: "right" },
              direction: "column",
              itemMarkWidth: 18,
              itemMarkHeight: 18,
              labelStyle: { fontSize: 14, fontWeight: 500, marginLeft: 6 },
            }}
          >
            <PieCenterLabel>Status</PieCenterLabel>
          </PieChart>
        )}
      </Box>
    </div>
  );
}
