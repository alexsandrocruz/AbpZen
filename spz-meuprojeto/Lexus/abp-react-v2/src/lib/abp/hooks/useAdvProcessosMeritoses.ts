import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProcessosMeritosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProcessosMeritoses(input: GetAdvProcessosMeritosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProcessosMeritoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos-meritos", {
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

export function useAllAdvProcessosMeritoses() {
  return useQuery({
    queryKey: ["advProcessosMeritoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos-meritos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProcessosMeritos(id: string) {
  return useQuery({
    queryKey: ["advProcessosMeritos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-processos-meritos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProcessosMeritos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-processos-meritos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosMeritoses"] });
    },
  });
}

export function useUpdateAdvProcessosMeritos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-processos-meritos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosMeritoses"] });
      queryClient.invalidateQueries({ queryKey: ["advProcessosMeritos", data.id] });
    },
  });
}

export function useDeleteAdvProcessosMeritos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-processos-meritos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosMeritoses"] });
    },
  });
}


