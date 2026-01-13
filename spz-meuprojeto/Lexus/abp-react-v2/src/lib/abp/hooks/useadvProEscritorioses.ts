import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProEscritoriosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProEscritorioses(input: GetAdvProEscritoriosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProEscritorioses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-escritorios", {
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

export function useAllAdvProEscritorioses() {
  return useQuery({
    queryKey: ["advProEscritorioses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-escritorios", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProEscritorios(id: string) {
  return useQuery({
    queryKey: ["advProEscritorios", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-escritorios/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProEscritorios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-escritorios", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProEscritorioses"] });
    },
  });
}

export function useUpdateAdvProEscritorios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-escritorios/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProEscritorioses"] });
      queryClient.invalidateQueries({ queryKey: ["advProEscritorios", data.id] });
    },
  });
}

export function useDeleteAdvProEscritorios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-escritorios/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProEscritorioses"] });
    },
  });
}


