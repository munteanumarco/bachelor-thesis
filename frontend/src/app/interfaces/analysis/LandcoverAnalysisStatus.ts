enum LandCoverAnalysisStatus {
  InProgress = 1,
  Completed = 2,
  Failed = 3,
}

const landCoverAnalysisStatusNames: {
  [key in LandCoverAnalysisStatus]: string;
} = {
  [LandCoverAnalysisStatus.InProgress]: 'In Progress',
  [LandCoverAnalysisStatus.Completed]: 'Completed',
  [LandCoverAnalysisStatus.Failed]: 'Failed',
};

const getLandCoverAnalysisStatusName = (
  severity: LandCoverAnalysisStatus
): string => landCoverAnalysisStatusNames[severity];

export {
  LandCoverAnalysisStatus,
  getLandCoverAnalysisStatusName,
  landCoverAnalysisStatusNames,
};
