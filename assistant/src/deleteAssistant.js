const OpenAI = require("openai");

function getDeleteAssistantParamsTemplate() {
  return {
    assistantId: "",
  };
}

async function deleteAssistant(params) {
  const secretKey = process.env.OPENAI_API_KEY;
  if (!secretKey) {
    console.error("OpenAI API key is missing.");
    return;
  }

  const openai = new OpenAI({
    apiKey: secretKey,
  });

  try {
    return await openai.beta.assistants.del(params.assistantId);
  } catch (error) {
    console.error("Error while deleting thread", error);
  }
}

module.exports = { getDeleteAssistantParamsTemplate, deleteAssistant };
