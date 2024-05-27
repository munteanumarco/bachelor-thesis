const OpenAI = require("openai");

async function createThread() {
  const secretKey = process.env.OPENAI_API_KEY;
  if (!secretKey) {
    logger.error("OpenAI API key is missing.");
    return;
  }
  const openai = new OpenAI({
    apiKey: secretKey,
  });

  try {
    const threadDetails = await openai.beta.threads.create();
    return threadDetails;
  } catch (error) {
    console.error("Error in thread creation", error);
  }
}

module.exports = { createThread };
