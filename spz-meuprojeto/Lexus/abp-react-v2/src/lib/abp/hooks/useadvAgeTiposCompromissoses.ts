import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvAgeTiposCompromissosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvAgeTiposCompromissoses(input: GetAdvAgeTiposCompromissosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advAgeTiposCompromissoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-age-tipos-compromissos", {
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

export function useAllAdvAgeTiposCompromissoses() {
  return useQuery({
    queryKey: ["advAgeTiposCompromissoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-age-tipos-compromissos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvAgeTiposCompromissos(id: string) {
  return useQuery({
    queryKey: ["advAgeTiposCompromissos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-age-tipos-compromissos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvAgeTiposCompromissos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-age-tipos-compromissos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advAgeTiposCompromissoses"] });
    },
  });
}

export function useUpdateAdvAgeTiposCompromissos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-age-tipos-compromissos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advAgeTiposCompromissoses"] });
      queryClient.invalidateQueries({ queryKey: ["advAgeTiposCompromissos", data.id] });
    },
  });
}

export function useDeleteAdvAgeTiposCompromissos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-age-tipos-compromissos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advAgeTiposCompromissoses"] });
    },
  });
}


