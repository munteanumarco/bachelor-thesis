const OpenAI = require("openai");

function getDeleteThreadParamsTemplate() {
  return {
    threadId: "",
  };
}

async function deleteThread(params) {
  const secretKey = process.env.OPENAI_API_KEY;
  if (!secretKey) {
    console.error("OpenAI API key is missing.");
    return;
  }

  const openai = new OpenAI({
    apiKey: secretKey,
  });

  try {
    return await openai.beta.threads.del(params.threadId);
  } catch (error) {
    console.error("Error while deleting thread", error);
  }
}

module.exports = { getDeleteThreadParamsTemplate, deleteThread };
