# https://markelsanz14.medium.com/introduction-to-reinforcement-learning-part-3-q-learning-with-neural-networks-algorithm-dqn-1e22ee928ecd
import tensorflow as tf


class DQN(tf.keras.Model):
    def __init__(self, output_sz: int):
        super(DQN, self).__init__()
        self.dense1 = tf.keras.layers.Dense(128, activation="relu")
        self.dense2 = tf.keras.layers.Dense(128, activation="relu")
        self.dense3 = tf.keras.layers.Dense(128, activation="relu")
        self.dense4 = tf.keras.layers.Dense(output_sz, dtype=tf.float32)  # No activation

    def call(self, inputs, **kwargs):
        inputs = tf.reshape(inputs, shape=(1, 25))
        x = self.dense1(inputs)
        x = self.dense2(x)
        x = self.dense3(x)
        return self.dense4(x)


input_size = 25
output_size = 14
network = DQN(output_size)
network.compile()
optimizer = tf.keras.optimizers.Adam(1e-4)
mse = tf.keras.losses.MeanSquaredError()


@tf.function
def train_step(inputs, target_outputs):
    """Perform a training iteration on a batch of data sampled"""
    # Calculate targets.
    with tf.GradientTape() as tape:
        results = network(inputs)
        loss = mse(results, target_outputs)
    grads = tape.gradient(loss, network.trainable_variables)
    optimizer.apply_gradients(zip(grads, network.trainable_variables))
    return loss
