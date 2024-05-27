const OpenAI = require("openai");

function getThreadMessagesParamsTemplate() {
  return {
    threadId: "",
  };
}

async function getThreadMessages(params) {
  const secretKey = process.env.OPENAI_API_KEY;
  if (!secretKey) {
    console.error("OpenAI API key is missing.");
    return;
  }

  const openai = new OpenAI({
    apiKey: secretKey,
  });

  try {
    return await openai.beta.threads.messages.list(params.threadId);
  } catch (error) {
    console.error("Error while getting thread messages:", error);
  }
}

module.exports = { getThreadMessages, getThreadMessagesParamsTemplate };
