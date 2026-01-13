import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProSentencasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProSentencases(input: GetAdvProSentencasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProSentencases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-sentencas", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllAdvProSentencases() {
  return useQuery({
    queryKey: ["advProSentencases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-sentencas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProSentencas(id: string) {
  return useQuery({
    queryKey: ["advProSentencas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-sentencas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProSentencas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-sentencas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProSentencases"] });
    },
  });
}

export function useUpdateAdvProSentencas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-sentencas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProSentencases"] });
      queryClient.invalidateQueries({ queryKey: ["advProSentencas", data.id] });
    },
  });
}

export function useDeleteAdvProSentencas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-sentencas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProSentencases"] });
    },
  });
}


