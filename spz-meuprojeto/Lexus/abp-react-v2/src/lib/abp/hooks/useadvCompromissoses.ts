import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCompromissosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCompromissoses(input: GetAdvCompromissosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCompromissoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-compromissos", {
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

export function useAllAdvCompromissoses() {
  return useQuery({
    queryKey: ["advCompromissoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-compromissos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCompromissos(id: string) {
  return useQuery({
    queryKey: ["advCompromissos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-compromissos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCompromissos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-compromissos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCompromissoses"] });
    },
  });
}

export function useUpdateAdvCompromissos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-compromissos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCompromissoses"] });
      queryClient.invalidateQueries({ queryKey: ["advCompromissos", data.id] });
    },
  });
}

export function useDeleteAdvCompromissos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-compromissos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCompromissoses"] });
    },
  });
}


