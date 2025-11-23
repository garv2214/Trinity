import React from "react";
import { SafeAreaView, StyleSheet, Text, View, TouchableOpacity } from "react-native";

export default function App() {
  return (
    <SafeAreaView style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.title}>Trinity</Text>
        <Text style={styles.subtitle}>A Mythological Multiplayer Universe</Text>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Welcome Warrior!</Text>
        <Text style={styles.sectionText}>
          Explore game lore, track missions, and manage your character directly
          from the Trinity mobile app.
        </Text>
      </View>

      <TouchableOpacity style={styles.btn}>
        <Text style={styles.btnText}>Enter App</Text>
      </TouchableOpacity>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#0a0a0a",
    paddingHorizontal: 20,
    justifyContent: "center",
  },
  header: {
    marginBottom: 40,
  },
  title: {
    fontSize: 42,
    fontWeight: "700",
    color: "#FFD700",
    textAlign: "center",
  },
  subtitle: {
    fontSize: 16,
    color: "#aaaaaa",
    textAlign: "center",
    marginTop: 5,
  },
  section: {
    marginBottom: 60,
  },
  sectionTitle: {
    fontSize: 24,
    fontWeight: "600",
    color: "#ff4444",
    marginBottom: 10,
    textAlign: "center",
  },
  sectionText: {
    fontSize: 16,
    color: "#e0e0e0",
    textAlign: "center",
    lineHeight: 22,
  },
  btn: {
    backgroundColor: "#ff4444",
    paddingVertical: 15,
    borderRadius: 10,
    width: "70%",
    alignSelf: "center",
  },
  btnText: {
    color: "#ffffff",
    fontSize: 18,
    textAlign: "center",
    fontWeight: "600",
  },
});
